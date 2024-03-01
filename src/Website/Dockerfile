FROM node:12.16.1-alpine  as build
WORKDIR /tmp
COPY package.json /tmp/
RUN npm install
COPY . /tmp/
RUN yarn build --configuration=production

FROM nginx:alpine
COPY --from=build /tmp/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
