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
                    currentBuild.result = "FAILURE"
                    sh "echo Rollback to the last successful build"
                    build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${BUILD_NUMBER-1}"]]
                    error(err)
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
                    currentBuild.result = "FAILURE"
                    sh "echo Rollback to the last successful build"
                    build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${BUILD_NUMBER-1}"]]
                    error(err)
                }
            }
        }
        stage("Deploy") {
            steps {
                try {
                    build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${BUILD_NUMBER}"]]
                } catch (err) {
                    currentBuild.result = "FAILURE"
                    sh "echo Rollback to the last successful build"
                    build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${BUILD_NUMBER-1}"]]
                    error(err)
                }
            }
        }
    }
}
