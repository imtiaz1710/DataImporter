FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
RUN apt-get update && apt-get install -y nodejs
COPY ["DataImporter/*.csproj", "DataImporter/"]
COPY ["DataImporter.Data/*.csproj", "DataImporter.Data/"]
COPY ["DataImporter.Common/*.csproj", "DataImporter.Common/"]
COPY ["DataImporter.Membership/*.csproj", "DataImporter.Membership/"]
COPY ["DataImporter.Excel/*.csproj", "DataImporter.Excel/"]
RUN dotnet restore "DataImporter/DataImporter.csproj"
COPY . .
WORKDIR "/src/DataImporter"
RUN dotnet build "DataImporter.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DataImporter.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DataImporter.dll"]