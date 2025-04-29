pipeline {
    agent any
    environment {
        DOTNET_VERSION = '9.0' // Specify .NET version
        BUILD_CONFIGURATION = 'Release'
        OUTPUT_DIR = 'bin/${BUILD_CONFIGURATION}/net9.0' // Adjust based on your project structure
        ZIP_FILE = "build_output_${env.BRANCH_NAME}.zip" // Use branch name in the zip file
        SFTP_BASE_PATH = '/Artifacts/MediBook' // Base path on the SFTP server
        SFTP_BRANCH_PATH = "${SFTP_BASE_PATH}/${env.BRANCH_NAME}" // Full path for the branch
    }
    stages {
        stage('Test Docker'){
            steps {
                sh 'docker --version'
            }
        }
        stage('Checkout Code') {
            steps {
                sh 'git clean -fdx'
                checkout scm
            }
        }
        stage('Restore Dependencies') {
            steps {
                sh 'dotnet dev-certs https --trust'
                echo '================================================= Restore Dependencies ===============================================' 
                sh 'dotnet restore'
            }
        }
        stage('Build') {
            steps {
                echo '================================================= Build ===============================================' 
                sh 'dotnet build --configuration ${BUILD_CONFIGURATION}'
            }
        }
        stage('Run Unit Tests') {
            steps {
                echo '================================================= Run Unit Tests ===============================================' 
                sh 'dotnet test --configuration ${BUILD_CONFIGURATION} --no-build --verbosity normal'
            }
        }
        stage('Package DLLs') {
            steps {
                echo '================================================= Package DLLs ===============================================' 
                sh "zip -r ${ZIP_FILE} ${OUTPUT_DIR}"
            }
        }
        stage('Upload to External Share via SFTP') {
            steps {
                echo '================================================= Upload to External Share via SFTP ===============================================' 
                withCredentials([sshUserPrivateKey(credentialsId: 'jenkins_sftpgo', keyFileVariable: 'SSH_KEY', usernameVariable: 'SFTP_USER')]) {
                    sh """
                    sftp -i $SSH_KEY $SFTP_USER@sv-mediavault.local:16022 <<EOF
                    mkdir ${SFTP_BRANCH_PATH}
                    cd ${SFTP_BRANCH_PATH}
                    put ${ZIP_FILE}
                    bye
                    EOF
                    """
                }
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
