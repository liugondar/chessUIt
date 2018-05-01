﻿using System;
using System.Collections.Generic;

namespace ChessKing
{
    class King : Chess
    {
        public King()
        {
            this.IsKing = true;
        }
        #region Find enable chess square to move or eat and display
        public override void FindWayAndAutoChangeSquareIfNeeded(ChessSquare[,] board, int row, int col)
        {
            // Kiểm tra các ô phía trên 
            KiemTraONhapThanh(board, row, col);
            KiemTraCacOPhiaTren(board, row, col);
            KiemTraCacOPhiaDuoi(board, row, col);
            KiemTraOBenTrai(board, row, col);
            KiemTraOBenPhai(board, row, col);
        }

        private void KiemTraONhapThanh(ChessSquare[,] board, int row, int col)
        {
            // row= 7 col=4 is default location white king
            // row= 7 col=0 is default location white queen side castle
            // row= 7 col=7 is default location white king side castle
            if (board[row, col].Chess.Team == (int)ColorTeam.White)
            {
                if (Common.isWhiteKingMoved) return;
                if (Common.isWhiteKingChecked) return;

                bool isWhiteQueenSideCastleAvailable =
                    !Common.IsEmptyChessSquare(board, 7, 0)
                    && !Common.isLeftWhiteCastleMoved
                    && CheckAvailableQueenPath(board, row: 7, KnightCol: 1, BishopCol: 2,
                    QueenCol: 3, team: (int)ColorTeam.White);

                bool isWhiteKingSideCastleAvailable =
                    !Common.IsEmptyChessSquare(board, 7, 7)
                    && !Common.isRightWhiteCastleMoved
                    && CheckAvailableKingPath(board, row: 7, KnightCol: 6, BishopCol: 5,
                    team: (int)ColorTeam.White);

                if (isWhiteQueenSideCastleAvailable)
                    Common.ChangeBackgroundColorToCanMove(board, 7, 2);

                if (isWhiteKingSideCastleAvailable)
                    Common.ChangeBackgroundColorToCanMove(board, 7, 6);
            }
            // row= 0 col=4 is default location black king
            // row= 0 col=0 is default location black queen side castle
            // row= 0 col=7 is default location black king side castle
            else
            {
                if (Common.isBlackKingMoved) return;
                if (Common.isBlackKingChecked) return;

                bool isBlackQueenSideCastleAvailable =
                     !Common.IsEmptyChessSquare(board, 0, 0)
                     && !Common.isLeftBlackCastleMoved
                     && CheckAvailableQueenPath(board, row: 0, KnightCol: 1, BishopCol: 2, QueenCol: 3,
                     team: (int)ColorTeam.Black);

                bool isBlackKingSideCastleAvailable =
                    !Common.IsEmptyChessSquare(board, 0, 7)
                    && !Common.isRightBlackCastleMoved
                    && CheckAvailableKingPath(board, row: 0, KnightCol: 6, BishopCol: 5,
                    team: (int)ColorTeam.Black);

                if (isBlackQueenSideCastleAvailable)
                    Common.ChangeBackgroundColorToCanMove(board, 0, 2);

                if (isBlackKingSideCastleAvailable)
                    Common.ChangeBackgroundColorToCanMove(board, 0, 6);
            }
        }
        private bool CheckAvailableQueenPath(ChessSquare[,] board, int row, int KnightCol, int BishopCol, int QueenCol, int team)
        {
            if (!Common.IsEmptyChessSquare(board, row, QueenCol)) return false;
            if (!Common.IsEmptyChessSquare(board, row, KnightCol)) return false;
            if (!Common.IsEmptyChessSquare(board, row, BishopCol)) return false;

            if (Common.IsDangerSquareToMove(board, row, QueenCol, team)) return false;
            if (Common.IsDangerSquareToMove(board, row, KnightCol, team)) return false;
            if (Common.IsDangerSquareToMove(board, row, BishopCol, team)) return false;

            return true;
        }
        private bool CheckAvailableKingPath(ChessSquare[,] board, int row, int KnightCol, int BishopCol, int team)
        {
            if (!Common.IsEmptyChessSquare(board, row, KnightCol)) return false;
            if (!Common.IsEmptyChessSquare(board, row, BishopCol)) return false;

            if (Common.IsDangerSquareToMove(board, row, KnightCol, team)) return false;
            if (Common.IsDangerSquareToMove(board, row, BishopCol, team)) return false;

            return true;
        }

        private void KiemTraOBenPhai(ChessSquare[,] board, int row, int col)
        {
            if (col < Constants.lastColOfTable)
            {
                // Kiểm tra ô bên phải nếu không ở cột đầu tiên
                ThayDoiONeuCanThiet(board, row, col + 1);
            }
        }
        private void KiemTraOBenTrai(ChessSquare[,] board, int row, int col)
        {
            if (col > Constants.firstColOfTable)
            {
                // Kiểm tra cột bên trái nếu không ở cột đầu tiên
                ThayDoiONeuCanThiet(board, row, col - 1);
            }
        }
        private void KiemTraCacOPhiaDuoi(ChessSquare[,] board, int row, int col)
        {
            if (row < Constants.lastRowOfTable)
            {
                //Kiểm tra ô chéo bên trái phía dưới nếu không ở vị trí cột đầu tiên
                if (col > Constants.firstColOfTable)
                    ThayDoiONeuCanThiet(board, row + 1, col - 1);

                // Kiểm tra ô chéo bên phải phía dưới nếu không ở vị trí cột cuối cùng
                if (col < Constants.lastColOfTable)
                    ThayDoiONeuCanThiet(board, row + 1, col + 1);
                // Kiểm tra ô phía dưới nếu không phải ở vị trí hàng cuối cùng
                if (row < Constants.lastColOfTable)
                    ThayDoiONeuCanThiet(board, row + 1, col);
            }
        }
        private void KiemTraCacOPhiaTren(ChessSquare[,] board, int row, int col)
        {
            if (row > Constants.firstRowOfTable)
            {
                //Kiểm tra ô chéo bên trái phía trên nếu vị trí không ở cột đầu tiên
                if (col > Constants.firstColOfTable)
                    ThayDoiONeuCanThiet(board, row - 1, col - 1);
                // Kiểm tra ô chéo bên phải phía trên nếu vị trí không ở cột cuối cùng
                if (col < Constants.lastColOfTable)
                    ThayDoiONeuCanThiet(board, row - 1, col + 1);
                //Kiểm tra ô phía bên trên nếu vị trí không ở hàng đầu tiên
                if (row > Constants.firstRowOfTable)
                    ThayDoiONeuCanThiet(board, row - 1, col);
            }
        }
        private void ThayDoiONeuCanThiet(ChessSquare[,] board, int row, int col)
        {
            if (Common.IsEmptyChessSquare(board, row, col))
            {
                if (!Common.IsDangerSquareToMove(board, row, col, this.Team))
                {
                    Common.ChangeBackgroundColorToCanMove(board, row, col);
                }
            }
            else
            {
                if (this.Team != board[row, col].Chess.Team)
                {
                    if (Common.IsProtected(board, row, col, board[row, col].Chess.Team)) return;
                    Common.ChangeBackgroundColorToCanEat(board, row, col);
                }
            }
        }
        #endregion

        #region Check chess square arround king
        /// <summary>
        /// return true if one  square arround king can be attack or move by teamate
        /// </summary>
        /// <param name="board"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool CheckSquareArroundCanBeAttackByTeamate(ChessSquare[,] board, int row, int col)
        {
            int enemyTeam = this.Team == (int)ColorTeam.White ?
                (int)ColorTeam.Black : (int)ColorTeam.White;

            bool result = CheckRightSquareCanBeAttackByTeamate(board, row, col, enemyTeam)
                || CheckLeftSquareCanbeAttackByTeamate(board, row, col, enemyTeam)
                || CheckBottomSquaresCanbeAttackByTeamate(board, row, col, enemyTeam)
                || CheckTopSquaresCanbeAttackByTeamate(board, row, col, enemyTeam);
            return result;
        }

        private bool CheckTopSquaresCanbeAttackByTeamate(ChessSquare[,] board, int row, int col, int enemyTeam)
        {
            if (row > Constants.firstRowOfTable)
            {
                // Square that Teamate can move=> dangerous for enemy to move
                if (col > Constants.firstColOfTable)
                    if (CheckSquareCanbeAttackByTeamate(board, row - 1, col - 1, enemyTeam)) return true;
                if (col < Constants.lastColOfTable)
                    if(CheckSquareCanbeAttackByTeamate(board, row - 1, col+1 , enemyTeam)) return true;
                if (row > Constants.firstRowOfTable)
                    if (CheckSquareCanbeAttackByTeamate(board, row - 1, col, enemyTeam)) return true;
            }
            return false;
        }
        private bool CheckBottomSquaresCanbeAttackByTeamate(ChessSquare[,] board, int row, int col, int enemyTeam)
        {
            if (row < Constants.lastRowOfTable)
            {
                // Square that Teamate can move=> dangerous for enemy to move

                if (col > Constants.firstColOfTable)
                    if (CheckSquareCanbeAttackByTeamate(board, row + 1, col - 1, enemyTeam)) return true;

                if (col < Constants.lastColOfTable)
                    if (CheckSquareCanbeAttackByTeamate(board, row + 1, col + 1, enemyTeam)) return true;

                if (row < Constants.lastColOfTable)
                    if (CheckSquareCanbeAttackByTeamate(board, row + 1, col, enemyTeam)) return true;
            }
            return false;
        }
        private bool CheckLeftSquareCanbeAttackByTeamate(ChessSquare[,] board, int row, int col, int enemyTeam)
        {
            if (col > Constants.firstColOfTable)
                // Square that Teamate can move=> dangerous for enemy to move
                if (CheckSquareCanbeAttackByTeamate(board, row, col - 1, enemyTeam)) return true;

            return false;
        }
        private bool CheckRightSquareCanBeAttackByTeamate(ChessSquare[,] board, int row, int col, int enemyTeam)
        {
            if (col < Constants.lastColOfTable)
                // Square that Teamate can move=> dangerous for enemy to move
                if (CheckSquareCanbeAttackByTeamate(board, row, col + 1, enemyTeam)) return true;

            return false;
        }

        private bool CheckSquareCanbeAttackByTeamate(ChessSquare[,] board, int row, int col, int enemyTeam)
        {
            bool result;
            if (board[row, col].Chess == null)
                result=Common.IsDangerSquareToMove(board, row, col, enemyTeam);
            else

            result=Common.IsSquareCanBeEatByEnemy(board, row, col, enemyTeam);

            return result;
        }

        #endregion
    }
}
