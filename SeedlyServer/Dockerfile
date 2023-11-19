# Use an official Golang runtime as a parent image
FROM golang:latest

# create a working directory inside the image
WORKDIR /app

# copy Go modules and dependencies to image
COPY SeedlyServer/go.mod ./

# download Go modules and dependencies
RUN go mod download

# copy directory files i.e all files ending with .go
COPY SeedlyServer/proto/*.go ./proto/
COPY SeedlyServer/main.go ./
RUN go get seedlyserver

# compile application
RUN go build

# tells Docker that the container listens on specified network ports at runtime
EXPOSE 50005

# command to be used to execute when the image is used to start a container
# Command to run your application
CMD ["./seedlyserver"]