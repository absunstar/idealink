
export interface cQuestionList {
    totalCount: number;
    lstResult: cQuestionItem[];
    pageSize: number;
}
export class cQuestionItem {
    Id: string;
    Name: string;
    IsActive: boolean;
    Difficulty: number;
    TrainingTypeId: string;
    TrainingCategoryName:string;
    TrainingCategoryId: string;
    TrainingTypeName:string;
    Answer: cAnswerItem[];

}
export class cExamQuestionList {
    questions: cQuestionItemTemplate[];
    ExamId: string;
}
export class cQuestionItemTemplate extends cQuestionItem{
   
    SelectedAnswer: string;

}
export class cAnswerItem {
    _id:string;
    Name: string;
    IsCorrectAnswer: boolean;
}
