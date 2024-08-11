//*****************************************************************************
//** 1568. Minimum Number of Days to Disconnect Island                       **
//*****************************************************************************
//*****************************************************************************

// Directions for moving in the grid
int directions[4][2] = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};

// DFS to mark visited cells
void dfs(int** grid, int gridSize, int* gridColSize, int x, int y, int** visited) {
    visited[x][y] = 1;
    for (int i = 0; i < 4; i++) {
        int nx = x + directions[i][0];
        int ny = y + directions[i][1];
        if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridColSize[nx] && grid[nx][ny] == 1 && !visited[nx][ny]) {
            dfs(grid, gridSize, gridColSize, nx, ny, visited);
        }
    }
}

// Check if the grid is connected (has exactly one island)
int isConnected(int** grid, int gridSize, int* gridColSize) {
    int** visited = (int**)malloc(gridSize * sizeof(int*));
    for (int i = 0; i < gridSize; i++) {
        visited[i] = (int*)malloc(gridColSize[i] * sizeof(int));
        memset(visited[i], 0, gridColSize[i] * sizeof(int));
    }
    
    int islandCount = 0;
    for (int i = 0; i < gridSize; i++) {
        for (int j = 0; j < gridColSize[i]; j++) {
            if (grid[i][j] == 1 && !visited[i][j]) {
                if (++islandCount > 1) {
                    for (int k = 0; k < gridSize; k++) free(visited[k]);
                    free(visited);
                    return 0;
                }
                dfs(grid, gridSize, gridColSize, i, j, visited);
            }
        }
    }
    
    for (int i = 0; i < gridSize; i++) free(visited[i]);
    free(visited);
    
    return islandCount == 1;
}

// Function to count the minimum days to disconnect the grid
int minDays(int** grid, int gridSize, int* gridColSize) {
    // Step 1: Check if the grid is initially disconnected
    if (!isConnected(grid, gridSize, gridColSize)) return 0;

    // Step 2: Try disconnecting by changing one cell
    for (int i = 0; i < gridSize; i++) {
        for (int j = 0; j < gridColSize[i]; j++) {
            if (grid[i][j] == 1) {
                grid[i][j] = 0;
                if (!isConnected(grid, gridSize, gridColSize)) {
                    grid[i][j] = 1;
                    return 1;
                }
                grid[i][j] = 1;
            }
        }
    }

    // Step 3: If one cell change doesn't work, try with two cells
    for (int i = 0; i < gridSize; i++) {
        for (int j = 0; j < gridColSize[i]; j++) {
            if (grid[i][j] == 1) {
                grid[i][j] = 0;
                for (int k = 0; k < gridSize; k++) {
                    for (int l = 0; l < gridColSize[k]; l++) {
                        if (grid[k][l] == 1) {
                            grid[k][l] = 0;
                            if (!isConnected(grid, gridSize, gridColSize)) {
                                grid[k][l] = 1;
                                grid[i][j] = 1;
                                return 2;
                            }
                            grid[k][l] = 1;
                        }
                    }
                }
                grid[i][j] = 1;
            }
        }
    }

    // Step 4: Return 2 days as the default if nothing worked (since the problem guarantees it can be done)
    return 2;
}
