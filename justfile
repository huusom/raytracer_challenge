# Justfile for Raytracer Challenge

# Default target
default:
	@just -l

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
	dotnet tool install dotnet-trace
	dotnet new install Reqnroll.Templates.DotNet

	echo "Setup complete!"


# run the cli
run PRESET="chapter9" OUT="out.ppm":
	dotnet run --project ./Raytracer/Raytracer.fsproj -- "{{PRESET}}" --out "{{OUT}}"

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

