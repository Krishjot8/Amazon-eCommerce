import { ProductVariant } from "./product-variant.model";
import { ProductImage } from "./product–image.model";

export interface Product {

  id: number;
  name: string;
description: string;

price: number;
stockQuantity: number;

type:string;
brand: string;
category: string;

mainImage: string;

images: ProductImage[];

variants: ProductVariant[];

rating: number;

reviewCount: number;

}
