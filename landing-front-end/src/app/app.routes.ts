import { Routes } from '@angular/router';
import { QuizComponent } from './quiz/quiz.component';
import { SolutionComponent } from './solution/solution.component';

export const routes: Routes = [
    {
        path: '',
        component: QuizComponent
    },
    {
        path: 'solution/:par',
        component: SolutionComponent
    }
];
