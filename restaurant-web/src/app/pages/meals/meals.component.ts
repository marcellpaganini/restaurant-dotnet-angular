import { Component, OnInit } from '@angular/core';
import { Meal } from '../../models/restaurant.models';
import { MealService } from '../../core/services/meal.service';

@Component({
  selector: 'app-meals',
  templateUrl: './meals.component.html',
  styleUrls: ['./meals.component.css']
})
export class MealsComponent implements OnInit {
  meals: Meal[] = [];
  loading = false;
  error = '';

  constructor(private readonly mealService: MealService) {}

  ngOnInit(): void {
    this.loadMeals();
  }

  loadMeals(): void {
    this.loading = true;
    this.error = '';
    this.mealService.getAll().subscribe({
      next: (meals) => {
        this.meals = meals;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message || 'Failed to load meals from the API.';
        this.loading = false;
      }
    });
  }
}
