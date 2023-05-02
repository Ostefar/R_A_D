pipeline {
    agent any
    environment {
        DEPLOY_NUMBER = "${BUILD_NUMBER}"
    }
    stages {
        stage("Build") {
            steps {
                sh "dotnet build"
                sh "docker-compose build"
            }
        }
        stage("Deliver") {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', passwordVariable: 'DH_PASSWORD', usernameVariable: 'DH_USERNAME')]) {
                    sh 'docker login -u $DH_USERNAME -p $DH_PASSWORD'
                    sh "docker-compose push"
                }
            }
        }
        stage("Deploy") {
            steps {
                try {
                    build job: 'RPS-Deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${DEPLOY_NUMBER}"]]
                } catch (e) {
                    echo "Error deploying: ${e}"
                    currentBuild.result = 'FAILURE'
                }
            }
        }
    }
}
