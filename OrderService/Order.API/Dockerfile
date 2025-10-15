# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Order.API/Order.API.csproj", "Order.API/"]
COPY ["Order.BusinessLogicLayer/Order.BusinessLogicLayer.csproj", "Order.BusinessLogicLayer/"]
COPY ["Order.DataAccessLayer/Order.DataAccessLayer.csproj", "Order.DataAccessLayer/"]
RUN dotnet restore "./Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/Order.API"
RUN dotnet build "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV MSSQL_DATABASE=OrdersDB
ENV MSSQL_HOST=sqlserver
ENV MSSQL_PORT=1433
ENV MSSQL_USER=sa
ENV MSSQL_PASSWORD=Password!
ENTRYPOINT ["dotnet", "Order.API.dll"]