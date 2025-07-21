# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["./HumanEvolution.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet publish "HumanEvolution.csproj" -c Release -o /app/out

# Étape finale : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# 👉 Railway attend que l'app écoute sur ce port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HumanEvolution.dll"]
