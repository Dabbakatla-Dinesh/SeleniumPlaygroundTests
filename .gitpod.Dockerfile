FROM gitpod/workspace-dotnet

# Set the workspace directory
WORKDIR /workspace

# Install basic dependencies with sudo
RUN sudo apt-get update && \
    sudo apt-get install -y curl git && \
    sudo apt-get clean && \
    sudo rm -rf /var/lib/apt/lists/*
