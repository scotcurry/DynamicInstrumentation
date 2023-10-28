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
        stage ('Get Git Version') {
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
                    sh "/usr/local/bin/docker build --tag docker.io/scotcurry4/dynamicinstrumentation:${current_version} \
                    --file ./DynamicInstrumentation/Dockerfile . \
                    --label org.opencontainers.image.revision=\"\$(git rev-parse HEAD)\" \
                    --label org.opencontainers.image.source=github.com/scotcurry/DynamicInstrumentation \
                    --build-arg passed_dd_version=${current_version}"
                }
            }
        }
        // stage ('Build / Update Datadog Service Catalog') {
        //     steps {
        //         sh '/opt/homebrew/bin/terraform init'
        //         sh '/opt/homebrew/bin/terraform plan -var datadog_app_key=${DATADOG_APP_KEY} -var datadog_api_key=${DATADOG_API_KEY}'
        //         sh '/opt/homebrew/bin/terraform apply -var datadog_app_key=${DATADOG_APP_KEY} -var datadog_api_key=${DATADOG_API_KEY} -auto-approve'
        //     }
        // }
    }
}
