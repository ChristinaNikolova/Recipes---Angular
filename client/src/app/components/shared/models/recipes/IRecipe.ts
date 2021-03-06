import IIngredient from "../ingredients/IIngredient";

export default interface IRecipe {
  title: string;
  content: string;
  picture: string;
  portions: number;
  preparationTime: number;
  cookingTime: number;
  categoryName: string;
  ingredients: Array<IIngredient>;
}
