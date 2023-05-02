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
                sh "dotnet build"
                sh "docker compose build"
            }
        }
        stage("Deliver") {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', passwordVariable: 'DH_PASSWORD', usernameVariable: 'DH_USERNAME')]) {
                    sh 'docker login -u $DH_USERNAME -p $DH_PASSWORD'
                    sh "docker compose push"
                }
            }
        }
        stage("Deploy") {
            steps {
                script {
                    def lastSuccessfulBuild = null
                    def lastSuccessfulDeploy = null
                    
                    // Find the last successful build and deploy numbers
                    for (int i = BUILD_NUMBER - 1; i > 0; i--) {
                        def build = Jenkins.instance.getItemByFullName('r_a_d').getBuildByNumber(i)
                        if (build.result == 'SUCCESS') {
                            lastSuccessfulBuild = i
                            break
                        }
                    }
                    
                    for (int i = BUILD_NUMBER - 1; i > 0; i--) {
                        def deploy = Jenkins.instance.getItemByFullName('r_a_d-deploy').getBuildByNumber(i)
                        if (deploy.result == 'SUCCESS') {
                            lastSuccessfulDeploy = i
                            break
                        }
                    }
                    
                    if (lastSuccessfulBuild == null || lastSuccessfulDeploy == null) {
                        sh "echo Cannot rollback - no previous successful build or deploy found"
                        currentBuild.result = "FAILURE"
                    } else if (lastSuccessfulDeploy >= lastSuccessfulBuild) {
                        sh "echo Rollback to deploy ${lastSuccessfulDeploy}"
                        build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${lastSuccessfulDeploy}"]]
                    } else {
                        sh "echo Rollback to build ${lastSuccessfulBuild}"
                        build job: 'r_a_d-deploy', parameters: [[$class: 'StringParameterValue', name: 'DEPLOY_NUMBER', value: "${lastSuccessfulBuild}"]]
                    }
                }
            }
        }
    }
}
