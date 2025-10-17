FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY WebApplication1/*.csproj WebApplication1/
COPY BusinessLogic/*.csproj BusinessLogic/
COPY Domain/*.csproj Domain/
COPY jlnest/*.csproj jlnest/

RUN dotnet restore "WebApplication1/WebApplication1.csproj"

# Устанавливаем EF Core Tools в build-стадии
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .

FROM build AS publish
RUN dotnet publish "WebApplication1/WebApplication1.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "WebApplication1.dll"]