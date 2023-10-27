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
                sh '/usr/local/microsoft/powershell/7/pwsh ./BuildCurrentVersion.ps1'
            }
        }
        stage ('Get Git Version') {
            steps {
                script {
                    git_sha = sh(script: 'git rev-parse HEAD', returnStdout: true)
                    echo "Git SHA: ${git_sha}"
                }
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
