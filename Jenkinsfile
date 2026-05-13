pipeline {
    agent any

    environment {
        DOTNET_SDK_VERSION = '8.0'
        SOLUTION = 'HealthcareEPR.Dossier.sln'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore ${SOLUTION}'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build ${SOLUTION} --configuration Release --no-restore'
            }
        }

        stage('Unit Tests') {
            steps {
                // This step MUST succeed for the pipeline to continue
                sh 'dotnet test ${SOLUTION} --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage"'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish HealthcareEPR.Dossier.Api/HealthcareEPR.Dossier.Api.csproj --configuration Release --no-build --output ./publish'
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: 'publish/**', allowEmptyArchive: true
            junit '**/TestResults/*.xml'
        }
        failure {
            echo 'Pipeline failed. Please check the unit test results.'
        }
    }
}
