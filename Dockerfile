FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["chat_api.csproj", "./"]
RUN dotnet restore "chat_api.csproj"
COPY . .
WORKDIR "/src/chat_api"
RUN dotnet build "chat_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "chat_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "chat_api.dll"]
