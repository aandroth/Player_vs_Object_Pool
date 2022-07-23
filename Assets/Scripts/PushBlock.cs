using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushBlock : MonoBehaviour
{
    public float unitsToMove, unitsMoved = 0, moveSpeed;
    public int currentPositionX, minPositionX, maxPositionX;
    public int currentPositionY, minPositionY, maxPositionY;
    public bool canMoveVert, canMoveHori;
    public Vector2 moveDir, oldPosition;
    public bool canMoveUp, canMoveDn, canMoveLf, canMoveRt;
    public BoxCollider2D collisionChecker;

    public enum PUSH_BLOCK_STATE {STANDING, MOVING};
    public PUSH_BLOCK_STATE state;

    private int startingPosX, startingPosY;

    private Vector2 startingCoords;

    public void Start()
    {
        state = PUSH_BLOCK_STATE.STANDING;

        startingPosX = currentPositionX;
        startingPosY = currentPositionY;

        startingCoords = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public void Update()
    {
        switch (state)
        {
            case PUSH_BLOCK_STATE.STANDING:
                break;
            case PUSH_BLOCK_STATE.MOVING:
                moveBlock();
                break;
        }
    }

    public void pushedUp()
    {
        if (canMoveUp && state == PUSH_BLOCK_STATE.STANDING && currentPositionY < maxPositionY && areaIsClear(new Vector2(0, 1)))
        {
            moveDir = new Vector2(0, 1);
            oldPosition = gameObject.transform.position;
            state = PUSH_BLOCK_STATE.MOVING;
            currentPositionY += 1;
        }
    }
    public void pushedDown()
    {
        if (canMoveDn && state == PUSH_BLOCK_STATE.STANDING && currentPositionY > minPositionY && areaIsClear(new Vector2(0, -1)))
        {
            moveDir = new Vector2(0, -1);
            oldPosition = gameObject.transform.position;
            state = PUSH_BLOCK_STATE.MOVING;
            currentPositionY -= 1;
        }
    }
    public void pushedLeft()
    {
        if(canMoveLf && state == PUSH_BLOCK_STATE.STANDING && currentPositionX > minPositionX && areaIsClear(new Vector2(-1, 0)))
        {
            moveDir = new Vector2(-1, 0);
            oldPosition = gameObject.transform.position;
            state = PUSH_BLOCK_STATE.MOVING;
            currentPositionX -= 1;
        }
    }
    public void pushedRight()
    {
        if (canMoveRt && state == PUSH_BLOCK_STATE.STANDING && currentPositionX < maxPositionX && areaIsClear(new Vector2(1, 0)))
        {
            moveDir = new Vector2(1, 0);
            oldPosition = gameObject.transform.position;
            state = PUSH_BLOCK_STATE.MOVING;
            currentPositionX += 1;
        }
    }

    public void changeCanMoveUp(bool b) { canMoveUp = b; }
    public void changeCanMoveDn(bool b) { canMoveDn = b; }
    public void changeCanMoveLf(bool b) { canMoveLf = b; }
    public void changeCanMoveRt(bool b) { canMoveRt = b; }

    private void moveBlock()
    {
        unitsMoved += Time.deltaTime * moveSpeed;

        if (unitsMoved >= unitsToMove)
        {
            gameObject.transform.position = oldPosition + (moveDir * unitsToMove);
            state = PUSH_BLOCK_STATE.STANDING;
            unitsMoved = 0;

            if (GetComponentInParent<PuzzleHolder>())
            {
                Debug.Log("GetComponentInParent<PuzzleHolder>().checkIfPuzzleBecameSolved()");
                GetComponentInParent<PuzzleHolder>().checkIfPuzzleBecameSolved();
            }
        }
        else
        {
            gameObject.transform.position = oldPosition + (moveDir * unitsMoved);
        }
        collisionChecker.transform.position = new Vector3(oldPosition.x + moveDir.x, oldPosition.y + moveDir.y, transform.position.z);
    }

    public void resetBlockToStartingCoordsAndPos()
    {
        Debug.Log("resetBlockToStartingCoordsAndPos()");
        transform.position = new Vector3(startingCoords.x, startingCoords.y, 0);
        currentPositionX = startingPosX;
        currentPositionY = startingPosY;
    }

    public void setCurrentCoordsAndPosAsStarting()
    {
        Debug.Log("setCurrentCoordsAndPosAsStarting()");
        startingCoords.Set(transform.position.x, transform.position.y);
        startingPosX = currentPositionX;
        startingPosY = currentPositionY;
    }

    private bool areaIsClear(Vector2 dir)
    {
        Vector2 checkPosition = (dir * unitsToMove);
        collisionChecker.transform.position = new Vector3(transform.position.x + checkPosition.x, transform.position.y + checkPosition.y, transform.position.z);
        Collider2D[] allOverlappingColliders = new Collider2D[16];
        Collider2D col2d = collisionChecker.GetComponent<Collider2D>();

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(collisionChecker.gameObject.layer));
        contactFilter.useLayerMask = true;

        int overlapCount = Physics2D.OverlapCollider(col2d, contactFilter, allOverlappingColliders);
        //var hitColliders = Physics2D.OverlapBoxAll(checkPosition, gameObject.GetComponent<Collider2D>().bounds.extents*2, 0);
        if (overlapCount == 0)
            return true;
        else
        {
            Debug.Log("overlapCount: " + overlapCount);
            collisionChecker.transform.position = transform.position;
            return false;
        }
    }

    public void DeactivateCollisionBoxes()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        collisionChecker.enabled = false;
    }

    public void ActivateCollisionBoxes()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        collisionChecker.enabled = true;
    }
}
