
class Point {
    constructor(i, j) {
        this.x = i
        this.y = j
        this.f = 0
        this.g = 0
        this.h = 0
        this.neighbors = undefined
        this.visited = false
        this.wall = true
        this.prev = undefined
    }
}

function findNeighbors(field, currentPoint, size) {
    const neighbors = []

    if (currentPoint.x > 0) {
        if (field[currentPoint.x - 1][currentPoint.y].wall != true) {
            neighbors.push(field[currentPoint.x - 1][currentPoint.y])
        }
    }

    if (currentPoint.y + 1 < size) {
        if (field[currentPoint.x][currentPoint.y + 1].wall != true) {
            neighbors.push(field[currentPoint.x][currentPoint.y + 1])
        }
    }

    if (currentPoint.y > 0) {
        if (field[currentPoint.x][currentPoint.y - 1].wall != true) {
            neighbors.push(field[currentPoint.x][currentPoint.y - 1])
        }
    }

    if (currentPoint.x + 1 < size) {
        if (field[currentPoint.x + 1][currentPoint.y].wall != true) {
            neighbors.push(field[currentPoint.x + 1][currentPoint.y])
        }
    }
    return neighbors
}

function calculateHeuristic(first, second) {
    return Math.abs(first.x - second.x) + Math.abs(first.y - second.y)
}

async function aStar(field, start, end) {
    const openSet = []
    openSet.push(start)
    const cameFrom = []

    while (openSet.length > 0) {
        if (stopAlgorithm === true) {
            return -1
        }
        let win = 0

        for (let i = 1; i < openSet.length; i++) {
            if (openSet[i].f < openSet[win].f) {
                win = i
            }

            if (openSet[i].f === openSet[win].f) {
                if (openSet[i].g > openSet[win].g) {
                    win = i
                }
            }
        }
        let currentNode = openSet[win]
        currentNode.visited = true

        if (currentNode == end) {
            return 1
        }

        deleteElement(openSet, currentNode)
        cameFrom.push(currentNode)

        let neighbors = findNeighbors(field, currentNode, size)

        for (const neighbor of neighbors) {
            if (!cameFrom.includes(neighbor)) {
                const g = currentNode.g + calculateHeuristic(currentNode, neighbor)

                if (!openSet.includes(neighbor)) {
                    openSet.push(neighbor)
                } else if (g >= neighbor.g) {
                    continue
                }

                neighbor.g = g
                neighbor.h = calculateHeuristic(neighbor, end)
                neighbor.f = neighbor.g + neighbor.h
                neighbor.prev = currentNode
            }
        }
    }
    
    return 0
}