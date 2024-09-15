# Use the .NET 8 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the project
RUN dotnet build -c Release -o out

# Publish the project to the /app/publish directory
RUN dotnet publish -c Release -o out

# Use the .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app/out .

# Define the entry point for the application
ENTRYPOINT ["dotnet", "gamestore.dll"]
