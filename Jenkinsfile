pipeline {
    agent any

    environment {
        DATADOG_API_KEY = credentials('DD_API_KEY')
        DATADOG_APP_KEY = credentials('DD_APP_KEY')
    }  
    stages {
        stage ('Get Comparison Variables') {
            steps  {
                script {
                    secrets_installed = sh(returnStdout: true, script:"/Users/scot.curry/google-cloud-sdk/bin/kubectl get secret | grep datadog-secret | awk '{ print \$1 }'").trim()
                    helm_chart_intalled = sh(returnStdout: true, script:"/opt/homebrew/bin/helm list --filter datadog | grep datadog | awk '{ print \$1 }'").trim()
                    deployment_exists = sh(returnStdout: true, script:"/Users/scot.curry/google-cloud-sdk/bin/kubectl get deployments | grep dynamicinstrumentation | awk '{ print \$1}'").trim()
                    machine_git_sha = sh(returnStdout: true, script:"/opt/homebrew/bin/git -C /Users/scot.curry/Projects/DynamicInstrumentation rev-parse HEAD").trim()
                    echo "secrets_installed value: ${secrets_installed}"
                    echo "helm_chart_intalled value: ${helm_chart_intalled}"
                    echo "deployment_exists value: ${deployment_exists}"
                }
            }
        }
        stage ('Github Checkout') {
            steps {
                script {
                    git branch: 'main',
                    url: 'https://github.com/scotcurry/DynamicInstrumentation.git'
                }
            }
        }
        stage ('Get Current Version') {
            steps {
                script {
                    def current_version_local = sh(returnStdout: true, script: '/usr/local/microsoft/powershell/7/pwsh ./BuildCurrentVersion.ps1')
                    current_version_local = current_version_local.trim()
                    env.current_version = current_version_local
                    echo "Current Version: ${current_version}" 
                }
            }
        }
        stage ('Get Git SHA Value') {
            steps {
                script {
                    git_sha = sh(returnStdout: true, script: 'git rev-parse HEAD').trim()
                    echo "Git SHA: ${git_sha}"
                    echo "Current Version 2: ${env.current_version}"
                }
            }
        }
        stage ('Update Dockerfile Template') {
            // when {
            //     not {
            //         equals expected: "${git_sha}", actual: "${machine_git_sha}"
            //     }
            // }
            steps {
                script {
                    sh "sed 's/<GIT_SHA>/${git_sha}/g' ./DynamicInstrumentation/Dockerfile-template > ./DynamicInstrumentation/Dockerfile"
                    sh 'cat ./DynamicInstrumentation/Dockerfile'
                }
            }
        }
        stage ('Build Docker Container') {
            // when {
            //     not {
            //         equals expected: "${git_sha}", actual: "${machine_git_sha}"
            //     }
            // }
            // If ever copying this code, make sure to get the quoting and escaping correct
            steps {
                script {
                   sh ("/usr/local/bin/docker build --tag docker.io/scotcurry4/dynamicinstrumentation:${current_version} \
                    --file ./DynamicInstrumentation/Dockerfile . \
                    --build-arg=DD_VERSION=${current_version}" )
                }
            }
        }
        stage ('Push Docker Image to Docker Hub') {
            // when {
            //     not {
            //         equals expected: "${git_sha}", actual: "${machine_git_sha}"
            //         }
            // }
            steps {
                script {
                    sh "/usr/local/bin/docker push scotcurry4/dynamicinstrumentation:${current_version}"
                }
            }
        }
        stage ('Create Deployment YAML File From Template') {
            steps {
                sh 'cp ./DynamicInstrumentation/dynamicinstrumentation-deployment-template.yaml ./DynamicInstrumentation/dynamicinstrumentation-starter.yaml'
                sh "sed 's/<DATADOG_VERSION>/${current_version}/g' ./DynamicInstrumentation/dynamicinstrumentation-starter.yaml  > ./DynamicInstrumentation/deployment-current-version.yaml"
                sh "sed 's/<GIT_SHA>/${git_sha}/g' ./DynamicInstrumentation/deployment-current-version.yaml > ./DynamicInstrumentation/deployment-git-sha.yaml"
                sh "sed 's/<DD_API_KEY>/${DATADOG_API_KEY}/g' ./DynamicInstrumentation/deployment-git-sha.yaml > ./DynamicInstrumentation/deployment-dd-api-key.yaml"
                sh "sed 's/<DD_APP_KEY>/${DATADOG_APP_KEY}/g' ./DynamicInstrumentation/deployment-dd-api-key.yaml > ./DynamicInstrumentation/deployment.yaml"
                sh 'cat ./DynamicInstrumentation/deployment.yaml'
            }
        }
        stage ('Install Kubernetes Secrets') {
            when {
                not {
                    equals expected: "datadog-secret", actual: "${secrets_installed}"
                }
            }
            steps {
                sh "/Users/scot.curry/google-cloud-sdk/bin/kubectl create secret generic datadog-secret --from-literal api-key=${DATADOG_API_KEY} --from-literal app-key=${DATADOG_APP_KEY}"
            }
        }
        stage ('Deploy Helm Charge') {
            when { 
                not { 
                    equals expected: "datadog", actual: helm_chart_intalled
                }
            }
            steps {
                sh '"/opt/homebrew/bin/helm install datadog datadog/datadog -f ./helm-values.yaml'
            }
        }
        stage ('Deploy App to Kubernetes') {
            // when {  
            //     equals expected: "dnamicinstrumentation", actual: "${deployment_exists}" 
            // }
            steps {
                sh '/Users/scot.curry/google-cloud-sdk/bin/kubectl apply -f ./DynamicInstrumentation/deployment.yaml'
            }
        }
        stage ('Build / Update Datadog Service Catalog') {
            steps {
                sh '/opt/homebrew/bin/terraform init'
                sh '/opt/homebrew/bin/terraform plan -var datadog_app_key=${DATADOG_APP_KEY} -var datadog_api_key=${DATADOG_API_KEY}'
                sh '/opt/homebrew/bin/terraform apply -var datadog_app_key=${DATADOG_APP_KEY} -var datadog_api_key=${DATADOG_API_KEY} -auto-approve'
            }
        }
    }
}
