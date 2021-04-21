FROM ubuntu:latest
LABEL maintainer="Tobias Bohn <info@tobiasbohn.com>"

ARG APP_PATH=/domicile/
ENV APP_PATH=$APP_PATH

WORKDIR $APP_PATH

RUN apt-get update -qq; \
  apt-get install -qq -y \
  zip \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/*

COPY server.zip $APP_PATH
COPY cert.json $APP_PATH
RUN unzip $APP_PATH/server.zip

RUN chmod +x $APP_PATH/server.x86_64

ENTRYPOINT $APP_PATH/server.x86_64
