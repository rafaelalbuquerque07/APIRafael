# Use a imagem oficial do .NET 6.0 SDK como base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Define o diretório de trabalho dentro do container
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o resto do código-fonte
COPY . ./

# Compila a aplicação
RUN dotnet publish -c Debug -o out

# Configura a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Instala ferramentas de desenvolvimento adicionais
RUN apt-get update && apt-get install -y \
    curl \
    git \
    vim \
    && rm -rf /var/lib/apt/lists/*

# Expõe a porta 5000 para a aplicação
EXPOSE 5000

# Define a variável de ambiente para desenvolvimento
ENV ASPNETCORE_ENVIRONMENT=Development

# Comando para executar a aplicação
ENTRYPOINT ["dotnet", "APIRafael.dll"]