FROM maven:3.9.11-eclipse-temurin-21-alpine AS builder
WORKDIR /app

COPY pom.xml mvnw ./
COPY .mvn .mvn
RUN chmod +x mvnw

RUN ./mvnw dependency:go-offline -B

COPY src ./src
RUN ./mvnw package -DskipTests -B

RUN mv target/MottuFlow-*.jar app.jar

FROM gcr.io/distroless/java21:nonroot AS rodador
WORKDIR /app

ENV DB_HOST=mottuflow-db \
    DB_PORT=3306 \
    DB_NAME=mottuflow \
    DB_USER=rm554874 \
    DB_PASSWORD=Mottuflow4874# \
    SERVER_PORT=8080

COPY --from=builder /app/app.jar app.jar

    EXPOSE 8080
USER nonroot
ENTRYPOINT ["java", "-jar", "app.jar"]