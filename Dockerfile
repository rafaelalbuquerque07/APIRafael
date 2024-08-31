# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia o arquivo de solução e restaura as dependências do NuGet
COPY APIRafael.sln ./
COPY APIRafael/*.csproj ./APIRafael/
RUN dotnet restore

# Copia todo o conteúdo da aplicação e realiza o build
COPY . .
WORKDIR /src/APIRafael
RUN dotnet build -c Release -o /app/build

# Publica a aplicação
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Criação da imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Define a variável de ambiente que aponta para a configuração de produção
ENV ASPNETCORE_ENVIRONMENT Production

# Expondo a porta que a aplicação irá rodar
EXPOSE 80

# Define o comando de entrada
ENTRYPOINT ["dotnet", "APIRafael.dll"]
