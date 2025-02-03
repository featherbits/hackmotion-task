FROM node:18-alpine as build
WORKDIR /app/src
COPY ./front-end ./
RUN npm ci
RUN npm run build

FROM node:18-alpine
WORKDIR /var/hackmotion
COPY --from=build /app/src/dist/landing-front-end ./
CMD node server/server.mjs