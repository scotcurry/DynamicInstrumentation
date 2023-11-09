pipeline {
    agent any

    environment {
        DATADOG_API_KEY = credentials('DD_API_KEY')
        DATADOG_APP_KEY = credentials('DD_APP_KEY')
    }  
    stages {
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
                    git_sha = sh(script: 'git rev-parse HEAD', returnStdout: true)
                    git_sha = git_sha.trim()
                    echo "Git SHA: ${git_sha}"
                    echo "Current Version 2: ${env.current_version}"
                }
            }
        }
        stage ('Build Docker Container') {
            // If ever copying this code, make sure to get the quoting and escaping correct
            steps {
                script {
                    sh ("/usr/local/bin/docker build --tag docker.io/scotcurry4/dynamicinstrumentation:${current_version} \
                    --file ./DynamicInstrumentation/Dockerfile . \
                    --build_arg DD_VERSION=${current_version} \
                    --build-arg DD_GIT_REPOSITORY_URL=github.com/scotcurry/DynamicInstrumentation \
                    --build-arg DD_GIT_COMMIT_SHA=${git_sha}" )
                }
            }
        }
        stage ('Push Docker Image to Docker Hub') {
            steps {
                script {
                    sh "/usr/local/bin/docker push scotcurry4/dynamicinstrumentation:${current_version}"
                }
            }
        }
        stage ('Create Deployment YAML File From Template') {
            steps {
                sh 'cp ./DynamicInstrumentation/dynamicinstrumentation-deployment-template.yaml ./DynamicInstrumentation/deployment.yaml'
                sh "sed 's/<DATADOG_VERSION>/${current_version}/g' ./DynamicInstrumentation/dynamicinstrumentation-deployment-template.yaml  > ./DynamicInstrumentation/deployment.yaml"
                sh 'cat ./DynamicInstrumentation/deployment.yaml'
            }
        }
        stage ('Deploy App to Kubernetes') {
            steps {
                sh '/Users/scot.curry/google-cloud-sdk/bin/kubectl create -f ./DynamicInstrumentation/deployment.yaml'
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
