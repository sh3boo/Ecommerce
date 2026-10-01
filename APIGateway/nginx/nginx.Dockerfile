FROM nginx

COPY APIGateway/nginx/nginx.local.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
