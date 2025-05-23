pipeline {
    agent any
    environment {
        DOTNET_VERSION = '9.0'
        BUILD_CONFIGURATION = 'Release'
        OUTPUT_DIR = 'PublishOutput/'
        ZIP_FILE = "build_output_${env.BRANCH_NAME}.zip"
        SFTP_BASE_COMPOSE_PATH = '/compose_files/MediBook'
        SFTP_BRANCH_COMPOSE_PATH = "${SFTP_BASE_COMPOSE_PATH}/${env.BRANCH_NAME}"
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'true'
        DOCKER_IMAGE = "registry.alphaepsilon.co.uk/medibook:${env.BRANCH_NAME}"
        SFTP_CREDENTIALS_ID = 'jenkins_sftpgo' // Assuming this is defined in Jenkins credentials
        SFTP_HOST = "sv-mediavault.local"
        SFTP_PORT = "16022"
    }
    stages {
        stage('Clean Workspace') {
            steps {
                echo '================================================= Clean Workspace ==============================================='
                deleteDir()
            }
        }
        stage('Checkout Code') {
            steps {
                echo '================================================= Git Checkout ==============================================='
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
        stage('Publish') {
            steps {
                echo '================================================= Publish ==============================================='
                dotnetPublish configuration: '${BUILD_CONFIGURATION}', noBuild: true, sdk: '.Net 9.0 SDK', selfContained: false
            }
        }
        stage('Build and Push Docker Image') {
            steps {
                echo '================================================= Build and Push Docker Image ==============================================='
                script {
                    // Define the registry URL and the credentials ID
                    def registryUrl = 'https://registry.alphaepsilon.co.uk' // Use https
                    def credentialsId = 'alphaepsilon-docker-registry' // This is the ID you'll define in Jenkins

                    // Use withRegistry to handle Docker login/logout
                    docker.withRegistry(registryUrl, credentialsId) {
                        def image = docker.build(env.DOCKER_IMAGE, "-f MediBook.Web/Dockerfile .")
                        image.push()
                    }
                }
            }
        }
        stage('Upload Docker Compose File') {
            steps {
                echo '================================================= Uploading Docker Compose File ==============================================='
                script {
                    // Local path to your docker-compose.yml in the Jenkins workspace
                    // ASSUMPTION: docker-compose.yml is in the root of your Git repo checkout.
                    // ADJUST THIS PATH if your docker-compose.yml is in a subdirectory (e.g., 'deploy/docker-compose.yml')
                    def localComposeFilePath = 'docker-compose.yml'

                    sshPublisher(
                        publishers: [
                            sshPublisherDesc(
                                configName: 'jenkins_sftpgo',
                                transfers: [
                                    sshTransfer(
                                        sourceFiles: "${localComposeFilePath}",
                                        remoteDirectory: "${SFTP_BRANCH_COMPOSE_PATH}"
                                    )
                                ]
                            )
                        ]
                    )
                    echo "Uploaded ${localComposeFilePath} to sftp://${env.SFTP_HOST}:${env.SFTP_PORT}${SFTP_BRANCH_COMPOSE_PATH}"
                }
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
            echo '------------------------------------------------ Build or deployment failed. -----------------------------------------------------'
        }
    }
}
