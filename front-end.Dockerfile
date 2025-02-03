FROM node:18-alpine as build
ARG AnalyticsServicePort
WORKDIR /app/src
COPY ./front-end ./
RUN sed -i "s/{analyticsServicePort}/${AnalyticsServicePort}/g" src/environments/environment.ts
RUN npm ci
RUN npm run build

FROM node:18-alpine
WORKDIR /var/hackmotion
COPY --from=build /app/src/dist/landing-front-end ./
CMD node server/server.mjs