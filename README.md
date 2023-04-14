# Back

## Prérequis
- Docker doit être installé sur votre machine.

## Configuration


1. Ouvrez un terminal et exécutez la commande `PS C:\Users\Dercraker> docker run -p 55555:80 --name PartyApi -h PartyApi -d dercraker0/api:dev` pour démarrer le container Docker.

2. Si tout se passe bien, un ID apparaîtra dans le terminal et l'API sera créée et accessible via `[localhost:55555/swagger](http://localhost:55555/swagger/index.html)`.

3. Notez que pour que les requêtes fonctionnent, le front-end doit être accessible via l'URL `http://localhost:8080` ou `http://localhost:8081`. Assurez-vous que le front-end est configuré en conséquence.

## Remarque

Assurez-vous d'avoir Docker installé sur votre machine avant de suivre les étapes ci-dessus. Si vous n'avez pas Docker, vous pouvez le télécharger à partir du site officiel : https://www.docker.com/products/docker-desktop
