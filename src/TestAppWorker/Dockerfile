FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/TestApp/TestAppWorker/TestAppWorker.csproj", "src/TestApp/TestAppWorker/"]
RUN dotnet restore "src/TestApp/TestAppWorker/TestAppWorker.csproj"
COPY . .
WORKDIR "/src/src/TestApp/TestAppWorker"
RUN dotnet build "TestAppWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAppWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAppWorker.dll"]