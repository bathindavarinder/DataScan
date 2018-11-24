pipeline {
  agent any
  stages {
    stage('Message') {
      steps {
        echo 'Checked'
      }
    }
    stage('') {
      steps {
        build(job: 'Build', wait: true)
      }
    }
  }
}