pipeline {
    agent any
    environment {
        DOTNET_VERSION = '9.0' // Specify .NET version
        BUILD_CONFIGURATION = 'Release'
    }
    stages {
        stage('Checkout Code') {
            steps {
                checkout scm
            }
        }
        stage('Restore Dependencies') {
            steps {
                sh 'dotnet dev-certs https --trust'
                sh 'dotnet restore'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build --configuration ${BUILD_CONFIGURATION}'
            }
        }
        stage('Run Unit Tests') {
            steps {
                sh 'dotnet test --no-build --verbosity normal'
            }
        }
    }
    post {
        always {
            echo 'Pipeline execution completed.'
        }
        success {
            echo 'Build and deployment succeeded.'
        }
        failure {
            echo 'Build or deployment failed.'
        }
    }
}