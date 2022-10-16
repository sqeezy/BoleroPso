/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("Run shell script") {
    container(displayName = "Build", image = "mcr.microsoft.com/dotnet/sdk:6.0-jammy") {
        shellScript {
            interpreter = "/bin/bash"
            content = """
                dotnet publish -c Release
            """
        }
    }
}
