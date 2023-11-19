# Use an official Golang runtime as a parent image
FROM golang:latest

# Set the working directory to /go/src/app
WORKDIR /go/src/app

# Copy the current directory contents into the container at /go/src/app
COPY . .

# Build the Go app
RUN go get -d -v ./...
RUN go install -v ./...

# Expose port 50005
EXPOSE 50005

# Command to run your application
CMD ["seedlyserver"]
