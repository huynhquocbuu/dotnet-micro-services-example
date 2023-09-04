```shell
docker-compose -f docker-compose.yml -f docker-compose-impl.yml up -d --remove-orphans --build
docker-compose -f docker-compose-all.yml -f docker-compose-all-impl.yml up -d --remove-orphans --build
```

