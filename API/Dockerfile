# Usa una imagen de Python ligera
FROM python:3.10-slim

# Establecer el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copiar archivos al contenedor
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copiar el código fuente
COPY . .

# Exponer el puerto en el que corre la API
EXPOSE 8000

# Comando para ejecutar la API
CMD ["python", "vulnerability_api.py"]

