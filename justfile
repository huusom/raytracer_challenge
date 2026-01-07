# Justfile for Raytracer Challenge

# Setup the development environment
setup:
	echo "Setting up development environment..."
	# Install Python dependencies
	curl -LsSf https://mistral.ai/vibe/install.sh | bash
	
	# Install .NET tools
	dotnet new tool-manifest
	dotnet tool install dotnet-format
	dotnet tool install fantomas 
	dotnet tool install dotnet-outdated-tool
	dotnet new install Reqnroll.Templates.DotNet

	echo "Setup complete!"


# Build the project
build:
	dotnet build

# Clean the project
clean:
	dotnet clean

# Format the code
format:
	dotnet format

# Run tests 
test:
	dotnet test

# Default target
default:
	just build