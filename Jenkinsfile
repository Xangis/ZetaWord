node {
	stage 'Checkout'
		checkout scm

	stage 'Build'
		bat 'nuget restore ZetaWord.sln'
		bat "\"${tool 'MSBuild'}\" ZetaWord.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

	stage 'Archive'
		archive 'ZetaWord/bin/Release/**'

}
