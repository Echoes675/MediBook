pipeline {
    agent any
    environment {
        DOTNET_VERSION = '9.0'
        BUILD_CONFIGURATION = 'Release'
        OUTPUT_DIR = 'BuildOutput/net9.0'
        ZIP_FILE = "build_output_${env.BRANCH_NAME}.zip"
        SFTP_BASE_PATH = '/Artifacts/MediBook'
        SFTP_BRANCH_PATH = "${SFTP_BASE_PATH}/${env.BRANCH_NAME}"
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'true'
    }
    stages {
        stage('Clean Workspace') {
            steps {
                deleteDir()
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
                zip zipFile: "${ZIP_FILE}", archive: false, dir: "${OUTPUT_DIR}"
            }
        }
        stage('Upload to External Share via SFTP') {
            steps {
                echo '================================================= Upload to External Share via SFTP ==============================================='
                publishOverSsh(
                    server: 'jenkins_sftpgo',
                    transferSet: [
                        [sourceFiles: "${ZIP_FILE}", remoteDirectory: "${SFTP_BRANCH_PATH}"]
                    ]
                )
            }
        }
    }
    post {
        always {
            echo '================================================= Pipeline execution completed. ================================================='
        }
        success {
            echo '++++++++++++++++++++++++++++++++++++++++++++++++ Build and deployment succeeded. +++++++++++++++++++++++++++++++++++++++++++++++++'
        }
        failure {
            echo '-------------------------------------------------- Build or deployment failed. ---------------------------------------------------'
        }
    }
}
