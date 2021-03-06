import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CategoriesService } from '../../../core/services/categories/categories.service';
import { RecipesService } from '../../../core/services/recipes/recipes.service';
import IUpdateRecipe from '../../shared/models/recipes/IUpdateRecipe';

const TITLE_MIN_LEN = 3;
const TITLE_MAX_LEN = 50;
const CONTENT_MIN_LEN = 3;
const CONTENT_MAX_LEN = 5000;
const PORTION_MIN = 1;
const PREP_MIN = 1;
const COOK_MIN = 1;
const INGREDIENT_NAME_MIN_LEN = 3;
const INGREDIENT_NAME_MAX_LEN = 50;
const INGREDIENT_QUANTITY_MIN_LEN = 3;
const INGREDIENT_QUANTITY_MAX_LEN = 50;

@Component({
  selector: 'app-update-recipe',
  templateUrl: './update-recipe.component.html',
  styleUrls: ['./update-recipe.component.css']
})
export class UpdateRecipeComponent implements OnInit {
  public categories$: Observable<Array<string>>;
  public updateForm: FormGroup;
  public id: string;
  public recipe: IUpdateRecipe;
  public ingredientsCount = 0;

  constructor(
    private fb: FormBuilder,
    private categoriesService: CategoriesService,
    private recipesService: RecipesService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.categories$ = this.categoriesService.getAllNames();
    this.buildForm();
  }

  public update(): void {
    if (this.updateForm.invalid) {
      return;
    }

    this.recipesService.update(this.updateForm.value, this.id).subscribe((_) => {
      this.router.navigate([`/recipe/details/${this.id}`]);
    });
  }

  public get f() {
    return this.updateForm.controls;
  }

  public get ingredients(): FormGroup {
    return this.updateForm.get('ingredients') as FormGroup;
  }

  public addIngredient() {
    const currentFormValue = this.updateForm.value;
    this.ingredientsCount++;
    this.buildForm();
    this.updateForm.patchValue(currentFormValue);
  }

  private buildForm(): void {
    this.recipe = this.route.snapshot.data['singleRecipe'];

    this.updateForm = this.fb.group({
      title: [this.recipe.title, [Validators.required, Validators.minLength(TITLE_MIN_LEN), Validators.maxLength(TITLE_MAX_LEN)]],
      content: [this.recipe.content, [Validators.required, Validators.minLength(CONTENT_MIN_LEN), Validators.maxLength(CONTENT_MAX_LEN)]],
      picture: [this.recipe.picture, [Validators.required]],
      portions: [this.recipe.portions, [Validators.required, Validators.min(PORTION_MIN)]],
      preparationTime: [this.recipe.preparationTime, [Validators.required, Validators.min(PREP_MIN)]],
      cookingTime: [this.recipe.cookingTime, [Validators.required, Validators.min(COOK_MIN)]],
      categoryName: [this.recipe.categoryName, [Validators.required]],
      ingredients: this.fb.array(
        new Array(this.ingredientsCount).fill(null).map((_, index) => (this.fb.group({
          name: ['', [Validators.required, Validators.minLength(INGREDIENT_NAME_MIN_LEN), Validators.maxLength(INGREDIENT_NAME_MAX_LEN)]],
          quantity: ['', [Validators.required, Validators.minLength(INGREDIENT_QUANTITY_MIN_LEN), Validators.maxLength(INGREDIENT_QUANTITY_MAX_LEN)]]
        })))
      )
    });
  }
}
