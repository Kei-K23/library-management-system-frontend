# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlazorTodoApp.csproj", "./"]
RUN dotnet restore "./BlazorTodoApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlazorTodoApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorTodoApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorTodoApp.dll"]