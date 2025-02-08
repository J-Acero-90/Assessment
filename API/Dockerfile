# Use a base image of Python
FROM python:3.9-slim-buster

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy the remaining source code and build the application
COPY . .

# Base image: .NET Runtime for running the application
EXPOSE 5000

# Set the entry point
CMD ["uvicorn", "vulnerability_api:app", "--host", "0.0.0.0", "--port", "5000" ]