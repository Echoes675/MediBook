pipeline {
    agent any
    environment {
        DOTNET_VERSION = '9.0' // Specify .NET version
        BUILD_CONFIGURATION = 'Release'
        OUTPUT_DIR = 'BuildOutput/net9.0' // Adjust based on your project structure
        ZIP_FILE = "build_output_${env.BRANCH_NAME}.zip" // Use branch name in the zip file
        SFTP_BASE_PATH = '/Artifacts/MediBook' // Base path on the SFTP server
        SFTP_BRANCH_PATH = "${SFTP_BASE_PATH}/${env.BRANCH_NAME}" // Full path for the branch
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'true'
    }
    stages {
         stage('Clean Workspace') {
             steps {
                 deleteDir() // Deletes all files in the workspace
             }
         }
         stage('Checkout Code') {
             steps {
                checkout scm
             }
         }
        stage('Clean Project') {
           steps {
               echo '================================================= Clean Project ===============================================' 
               sh 'git clean -fdx'
               dotnetClean configuration: "${BUILD_CONFIGURATION}", sdk: '.Net 9.0 SDK'
           }
        }
        stage('Restore Dependencies') {
            steps {
                echo '================================================= Restore Dependencies ===============================================' 
                dotnetRestore sdk: '.Net 9.0 SDK'
            }
        }
        stage('Build') {
            steps {
                echo '================================================= Build ===============================================' 
                dotnetBuild configuration: "${BUILD_CONFIGURATION}", sdk: '.Net 9.0 SDK'
            }
        }
        stage('Run Unit Tests') {
            steps {
                echo '================================================= Run Unit Tests ===============================================' 
                dotnetTest configuration: "${BUILD_CONFIGURATION}", noBuild: true, sdk: '.Net 9.0 SDK', verbosity: 'n'
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
