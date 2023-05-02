pipeline {
    agent any
    triggers {
        pollSCM("* * * * *")
    }
    environment {
        DEPLOY_NUMBER = "${BUILD_NUMBER}"
    }
    stages {
        stage("Build") {
            steps {
                try {
                    sh "dotnet build"
                    sh "docker compose build"
                } catch (err) {
                    error "Build stage failed. Error: ${err}"
                }
            }
        }
        stage("Deliver") {
            steps {
                try {
                    withCredentials([usernamePassword(credentialsId: 'DockerHub', passwordVariable: 'DH_PASSWORD', usernameVariable: 'DH_USERNAME')]) {
                        sh 'docker login -u $DH_USERNAME -p $DH_PASSWORD'
                        sh "docker compose push"
                    }
                } catch (err) {
                    error "Deliver stage failed. Error: ${err}"
                }
            }
        }
        stage("Deploy") {
            steps {
                try {
                    build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${BUILD_NUMBER}"]]
                } catch (err) {
                    error "Deploy stage failed. Error: ${err}"
                }
            }
        }
    }
}
