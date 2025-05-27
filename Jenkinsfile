pipeline {
    agent any
    options{
        disableConcurrentBuilds()
    }
    environment {
        DOTNET_VERSION = '9.0'
        BUILD_CONFIGURATION = 'Release'
        OUTPUT_DIR = 'PublishOutput/'
        ZIP_FILE = "build_output_${env.BRANCH_NAME}.zip"
        SFTP_BASE_COMPOSE_PATH = '/compose_files/MediBook'
        SFTP_BRANCH_COMPOSE_PATH = "${SFTP_BASE_COMPOSE_PATH}/${env.BRANCH_NAME}"
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'true'

        // --- MODIFIED ENVIRONMENT VARIABLE ---
        // The DOCKER_IMAGE tag will now directly use env.BRANCH_NAME
        // Be aware that Docker tags have some character restrictions.
        // If your branch names contain characters like '/' or ':', Docker might complain.
        // Common safe characters for tags: [a-zA-Z0-9.-_]
        // If your branch names ever contain '/', you'll need to use the previous 'BRANCH_TAG' sanitization.
        DOCKER_IMAGE_TAG = "${env.BRANCH_NAME}" // Directly use the branch name as the tag
        DOCKER_REGISTRY = "registry.alphaepsilon.co.uk"
        FULL_DOCKER_IMAGE = "${DOCKER_REGISTRY}/medibook:${DOCKER_IMAGE_TAG}" // Full image name with branch tag

        SFTP_CREDENTIALS_ID = 'jenkins_sftpgo'
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
                    def registryUrl = "https://${env.DOCKER_REGISTRY}"
                    def credentialsId = 'alphaepsilon-docker-registry'

                    echo "Building Docker image with tag: ${env.FULL_DOCKER_IMAGE}"

                    docker.withRegistry(registryUrl, credentialsId) {
                        // Use the FULL_DOCKER_IMAGE variable which contains the branch name as the tag
                        def image = docker.build(env.FULL_DOCKER_IMAGE, "-f MediBook.Web/Dockerfile .")

                        if (env.BRANCH_NAME == 'main' || env.BRANCH_NAME == 'master') {
                            echo "Also tagging with 'latest'"
                            image.tag("latest")
                        }

                        image.push()
                    }
                }
            }
        }
        stage('Upload Docker Compose File') {
            steps {
                echo '================================================= Uploading Docker Compose File ==============================================='
                script {
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