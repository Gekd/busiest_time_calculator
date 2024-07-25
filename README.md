# Busiest time calculator

Calculator used to determine when most workers take breaks.

## Steps to use this project:

1. Clone the repository:
    ```sh
    git clone https://github.com/Gekd/busiest_time_calculator.git
    ```

2. Navigate into the project directory:
    ```sh
    cd busiest_time_calculator
    ```

3. Build the Docker image:
    ```sh
    docker build -t busiest_time_calculator .
    ```

4. Run the Docker container:
    ```sh
    docker run -it busiest_time_calculator
    ```

   Or, if you want to use an external `.txt` file:

4. Run the Docker container with a volume mount:
    ```sh
    docker run -it -v "<path-to-your-txt-file>:/data" busiest_time_calculator
    ```
   
   Insert into command line:
    ```sh
    /data/(your .txt file name)
    ```

Time entry format `<start-time><end-time>` (i.e `11:1512:00`)

