# Back

## Prérequis
- Docker doit être installé sur votre machine.

## Configuration

1. Créez un dossier et placez-y le fichier `docker-compose.yml` avec le contenu suivant :

```yml
version: '3.9'
services:
  api:
    container_name: challenge-48h-api
    image: 'dercraker0/api:prod'
    restart: always

    ports:
      - '55555:80'
      
  db:
    container_name: challenge-48h-db
    image: 'mcr.microsoft.com/mssql/server:2022-latest'
    restart: always
    environment:
      SA_PASSWORD: "40e26X88960pMQkM5Af7"
      ACCEPT_EULA: 'Y'
      PID: Express
    user: root
    ports:
      - '1433:1433'
    volumes:
      - 'Party_data:/var/opt/mssql/data'
      - 'Party_log:/var/opt/mssql/log'
      - 'Party_backup:/var/opt/mssql/backup'
      
volumes:
  Party_data: null
  Party_log: null
  Party_backup: null'
```

## Configuration

2. Ouvrez un terminal et exécutez la commande `docker-compose up -d` pour démarrer les conteneurs Docker.

3. Si tout se passe bien, un ID apparaîtra dans le terminal et l'API sera créée et accessible via `localhost:55555/swagger`.

4. Notez que pour que les requêtes fonctionnent, le front-end doit être accessible via l'URL `http://localhost:8080` ou `http://localhost:8081`. Assurez-vous que le front-end est configuré en conséquence.

## Remarque

Assurez-vous d'avoir Docker installé sur votre machine avant de suivre les étapes ci-dessus. Si vous n'avez pas Docker, vous pouvez le télécharger à partir du site officiel : https://www.docker.com/products/docker-desktop
