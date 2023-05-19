# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.202.5/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 5.0, 3.1, 2.1
ARG VARIANT="7.0"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Option] Install Node.js
ARG INSTALL_NODE="false"
ARG NODE_VERSION="lts/*"
RUN if [ "${INSTALL_NODE}" = "true" ]; then su vscode -c "source /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install additional .NET Core SDKs or runtimes
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && nvm install 2>&1"

# RUN dotnet tool install --global dotnet-ef 
# --version 3.1.1
# RUN dotnet add package Newtonsoft.Json

